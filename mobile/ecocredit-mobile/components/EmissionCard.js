import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';

const STATUS_COLORS = {
  NORMAL:   '#00e87a',
  WARNING:  '#ffaa00',
  CRITICAL: '#ff4d6a',
};

export default function EmissionCard({ facility, onPress }) {
  const color = STATUS_COLORS[facility.status] ?? '#5a7a60';
  const pct   = Math.min(facility.percentUsed, 100);

  return (
    <TouchableOpacity style={styles.card} onPress={onPress} activeOpacity={0.8}>
      <View style={styles.header}>
        <Text style={styles.name}>{facility.name}</Text>
        <View style={[styles.badge, { backgroundColor: color + '22' }]}>
          <Text style={[styles.badgeText, { color }]}>{facility.status}</Text>
        </View>
      </View>

      <View style={styles.barTrack}>
        <View style={[styles.barFill, { width: `${pct}%`, backgroundColor: color }]} />
      </View>

      <View style={styles.footer}>
        <Text style={styles.detail}>
          {facility.emittedTco2e.toLocaleString()} / {facility.limitTco2.toLocaleString()} tCO₂e
        </Text>
        <Text style={[styles.pct, { color }]}>{facility.percentUsed}%</Text>
      </View>
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  card:      { backgroundColor: '#0b110e', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#1a2e22', marginBottom: 10 },
  header:    { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 },
  name:      { fontSize: 14, fontWeight: '700', color: '#ffffff', flex: 1 },
  badge:     { paddingHorizontal: 8, paddingVertical: 2, borderRadius: 20 },
  badgeText: { fontSize: 10, fontWeight: '700' },
  barTrack:  { height: 6, backgroundColor: '#1a2e22', borderRadius: 3, marginBottom: 8, overflow: 'hidden' },
  barFill:   { height: '100%', borderRadius: 3 },
  footer:    { flexDirection: 'row', justifyContent: 'space-between' },
  detail:    { fontSize: 11, color: '#5a7a60' },
  pct:       { fontSize: 11, fontWeight: '700' },
});
