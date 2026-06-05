import React, { useState } from 'react';
import {
  View, Text, TextInput, TouchableOpacity,
  StyleSheet, KeyboardAvoidingView, Platform, ActivityIndicator,
} from 'react-native';

// Credenciais demo — refletem os seeds do backend
const DEMO_CREDENTIALS = [
  { email: 'admin@petro.com',   password: 'Admin@2026',   role: 'ADMIN'   },
  { email: 'analyst@petro.com', password: 'Analyst@2026', role: 'ANALYST' },
];

export default function LoginScreen({ onLogin }) {
  const [email,    setEmail]    = useState('');
  const [password, setPassword] = useState('');
  const [error,    setError]    = useState('');
  const [loading,  setLoading]  = useState(false);

  const handleLogin = async () => {
    if (!email || !password) {
      setError('Preencha e-mail e senha.');
      return;
    }
    setLoading(true);
    setError('');

    // Simula latência de rede
    await new Promise(r => setTimeout(r, 800));

    const match = DEMO_CREDENTIALS.find(
      c => c.email === email.trim().toLowerCase() && c.password === password
    );

    setLoading(false);

    if (match) {
      onLogin({ email: match.email, role: match.role });
    } else {
      setError('E-mail ou senha inválidos.');
    }
  };

  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}>

      <View style={styles.inner}>
        <Text style={styles.logo}>🌿 EcoCredit</Text>
        <Text style={styles.subtitle}>Carbon Compliance Platform</Text>
        <Text style={styles.tagline}>FIAP Global Solution 2026</Text>

        <View style={styles.form}>
          <Text style={styles.label}>E-mail</Text>
          <TextInput
            style={styles.input}
            value={email}
            onChangeText={setEmail}
            placeholder="seu@email.com"
            placeholderTextColor="#3a5a40"
            autoCapitalize="none"
            keyboardType="email-address"
          />

          <Text style={styles.label}>Senha</Text>
          <TextInput
            style={styles.input}
            value={password}
            onChangeText={setPassword}
            placeholder="••••••••"
            placeholderTextColor="#3a5a40"
            secureTextEntry
          />

          {!!error && <Text style={styles.error}>{error}</Text>}

          <TouchableOpacity
            style={[styles.btn, loading && styles.btnDisabled]}
            onPress={handleLogin}
            disabled={loading}>
            {loading
              ? <ActivityIndicator color="#060a08" />
              : <Text style={styles.btnText}>Entrar</Text>}
          </TouchableOpacity>
        </View>

        <View style={styles.hintBox}>
          <Text style={styles.hintTitle}>Acesso demo</Text>
          <Text style={styles.hint}>admin@petro.com · Admin@2026</Text>
          <Text style={styles.hint}>analyst@petro.com · Analyst@2026</Text>
        </View>
      </View>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container:   { flex: 1, backgroundColor: '#060a08' },
  inner:       { flex: 1, justifyContent: 'center', paddingHorizontal: 28 },
  logo:        { fontSize: 34, fontWeight: '900', color: '#00e87a', textAlign: 'center', marginBottom: 6 },
  subtitle:    { fontSize: 15, color: '#ddeedd', textAlign: 'center', fontWeight: '600' },
  tagline:     { fontSize: 11, color: '#5a7a60', textAlign: 'center', marginBottom: 40, letterSpacing: 1, textTransform: 'uppercase' },
  form:        { gap: 8 },
  label:       { fontSize: 12, color: '#5a7a60', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 2 },
  input:       { backgroundColor: '#0b110e', borderWidth: 1, borderColor: '#1a2e22', borderRadius: 10, padding: 14, color: '#ffffff', fontSize: 15, marginBottom: 8 },
  error:       { color: '#ff4d6a', fontSize: 13, marginBottom: 4 },
  btn:         { backgroundColor: '#00e87a', borderRadius: 12, padding: 16, alignItems: 'center', marginTop: 8 },
  btnDisabled: { opacity: 0.6 },
  btnText:     { color: '#060a08', fontWeight: '800', fontSize: 16 },
  hintBox:     { marginTop: 40, padding: 16, backgroundColor: '#0b110e', borderRadius: 12, borderWidth: 1, borderColor: '#1a2e22' },
  hintTitle:   { fontSize: 11, color: '#5a7a60', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 6 },
  hint:        { fontSize: 12, color: '#3a7a50', fontFamily: Platform.OS === 'ios' ? 'Courier New' : 'monospace', marginBottom: 2 },
});
