import React from 'react';
import { View, Text, ScrollView, StyleSheet } from 'react-native';
import { MOCK_CREDITS, MOCK_COMPANY } from '../data/mockData';

const STATUS_COLORS = {
  AVAILABLE: '#00e87a',
  USED:      '#5a7a60',
  EXPIRED:   '#ff4d6a',
};

const TYPE_LABELS = {
  CBIO:   'CBIO',
  REDD:   'REDD+',
  EU_ETS: 'EU ETS',
  VCM:    'VCM',
  GOLD:   'Gold',
};

export default function CreditsScreen() {
  const available = MOCK_CREDITS
    .filter(c => c.status === 'AVAILABLE')
    .reduce((sum, c) => sum + c.quantity, 0);

  return (
    <ScrollView style={styles.container} showsVerticalScrollIndicator={false}>
      <View style={styles.header}>
        <Text style={styles.headerSub}>EcoCredit · Créditos de Carbono</Text>
        <Text style={styles.headerTitle}>Portfólio ESG</Text>
      </View>

      <View style={styles.balanceCard}>
        <Text style={styles.balanceLabel}>Saldo Disponível</Text>
        <Text style={styles.balanceValue}>{available.toLocaleString()} tCO₂</Text>
        <Text style={styles.balanceSub}>Créditos AVAILABLE</Text>
      </View>

      <Text style={styles.sectionTitle}>Portfólio de Créditos</Text>

      {MOCK_CREDITS.map(c => (
        <View key={c.creditId} style={styles.creditCard}>
          <View style={styles.creditHeader}>
            <View>
              <Text style={styles.creditType}>{TYPE_LABELS[c.type] || c.type}</Text>
              <Text style={styles.creditQty}>{c.quantity.toLocaleString()} tCO₂</Text>
            </View>
            <View style={[styles.badge, { backgroundColor: STATUS_COLORS[c.status] + '25' }]}>
              <Text style={[styles.badgeText, { color: STATUS_COLORS[c.status] }]}>{c.status}</Text>
            </View>
          </View>
          <View style={styles.creditDetails}>
            <View style={styles.detailItem}>
              <Text style={styles.detailLabel}>Preço</Text>
              <Text style={styles.detailValue}>US$ {c.priceUsd.toFixed(2)}/t</Text>
            </View>
            <View style={styles.detailItem}>
              <Text style={styles.detailLabel}>Valor Total</Text>
              <Text style={styles.detailValue}>US$ {(c.quantity * c.priceUsd).toLocaleString()}</Text>
            </View>
            <View style={styles.detailItem}>
              <Text style={styles.detailLabel}>Expiração</Text>
              <Text style={styles.detailValue}>{c.expiry}</Text>
            </View>
          </View>
        </View>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container:     { flex: 1, backgroundColor: '#060a08' },
  header:        { padding: 24, paddingTop: 60 },
  headerSub:     { fontSize: 11, color: '#5a7a60', letterSpacing: 2, textTransform: 'uppercase', marginBottom: 4 },
  headerTitle:   { fontSize: 22, fontWeight: '800', color: '#ffffff' },
  balanceCard:   { margin: 16, padding: 28, backgroundColor: '#0b2e1a', borderRadius: 16, borderWidth: 1, borderColor: '#00e87a40', alignItems: 'center' },
  balanceLabel:  { fontSize: 12, color: '#5a7a60', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 8 },
  balanceValue:  { fontSize: 42, fontWeight: '900', color: '#00e87a' },
  balanceSub:    { fontSize: 12, color: '#5a7a60', marginTop: 6 },
  sectionTitle:  { fontSize: 13, fontWeight: '700', color: '#00e87a', marginHorizontal: 16, marginBottom: 10, textTransform: 'uppercase', letterSpacing: 2 },
  creditCard:    { marginHorizontal: 16, marginBottom: 10, backgroundColor: '#0b110e', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#1a2e22' },
  creditHeader:  { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: 14 },
  creditType:    { fontSize: 16, fontWeight: '800', color: '#ffffff', marginBottom: 4 },
  creditQty:     { fontSize: 22, fontWeight: '900', color: '#00e87a' },
  badge:         { paddingHorizontal: 10, paddingVertical: 4, borderRadius: 20 },
  badgeText:     { fontSize: 10, fontWeight: '700' },
  creditDetails: { flexDirection: 'row', justifyContent: 'space-between' },
  detailItem:    { alignItems: 'center' },
  detailLabel:   { fontSize: 10, color: '#5a7a60', marginBottom: 2 },
  detailValue:   { fontSize: 12, color: '#ddeedd', fontWeight: '600' },
});
