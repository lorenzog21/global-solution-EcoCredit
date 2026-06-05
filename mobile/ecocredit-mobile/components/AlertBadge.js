import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

const SEVERITY_CONFIG = {
  CRITICAL: { color: '#ff4d6a', bg: '#ff4d6a22', icon: '🚨' },
  HIGH:     { color: '#ffaa00', bg: '#ffaa0022', icon: '⚠️' },
  MEDIUM:   { color: '#4da6ff', bg: '#4da6ff22', icon: 'ℹ️' },
  LOW:      { color: '#00e87a', bg: '#00e87a22', icon: '✅' },
};

export default function AlertBadge({ alert, compact = false }) {
  const cfg = SEVERITY_CONFIG[alert.severity] ?? SEVERITY_CONFIG.LOW;

  if (compact) {
    return (
      <View style={[styles.compactBadge, { backgroundColor: cfg.bg, borderColor: cfg.color }]}>
        <Text style={[styles.compactText, { color: cfg.color }]}>
          {cfg.icon} {alert.severity}
        </Text>
      </View>
    );
  }

  return (
    <View style={[styles.card, { borderLeftColor: cfg.color }]}>
      <View style={styles.row}>
        <Text style={[styles.severity, { color: cfg.color }]}>{cfg.icon} {alert.severity}</Text>
        <Text style={styles.time}>{alert.time}</Text>
      </View>
      <Text style={styles.message}>{alert.message}</Text>
      <Text style={styles.meta}>{alert.facility}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  card:         { backgroundColor: '#0b110e', borderRadius: 10, padding: 14, borderWidth: 1, borderColor: '#1a2e22', borderLeftWidth: 3, marginBottom: 8 },
  row:          { flexDirection: 'row', justifyContent: 'space-between', marginBottom: 4 },
  severity:     { fontSize: 10, fontWeight: '700', letterSpacing: 1 },
  time:         { fontSize: 10, color: '#5a7a60' },
  message:      { fontSize: 13, color: '#ddeedd', marginBottom: 4 },
  meta:         { fontSize: 11, color: '#5a7a60' },
  compactBadge: { paddingHorizontal: 8, paddingVertical: 3, borderRadius: 20, borderWidth: 1 },
  compactText:  { fontSize: 10, fontWeight: '700' },
});
