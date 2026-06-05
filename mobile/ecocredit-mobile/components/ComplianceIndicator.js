import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

const THRESHOLDS = [
  { min: 80, label: 'Em Conformidade', color: '#00e87a' },
  { min: 60, label: 'Atenção',         color: '#ffaa00' },
  { min: 0,  label: 'Fora de Padrão',  color: '#ff4d6a' },
];

export default function ComplianceIndicator({ score, size = 'md' }) {
  const { label, color } = THRESHOLDS.find(t => score >= t.min) ?? THRESHOLDS[2];
  const isLarge = size === 'lg';

  return (
    <View style={[styles.wrapper, isLarge && styles.wrapperLg]}>
      <Text style={[styles.score, { color }, isLarge && styles.scoreLg]}>
        {score}%
      </Text>
      <View style={[styles.badge, { backgroundColor: color + '22', borderColor: color }]}>
        <Text style={[styles.label, { color }]}>{label}</Text>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  wrapper:  { alignItems: 'center', gap: 6 },
  wrapperLg: { gap: 10 },
  score:    { fontSize: 36, fontWeight: '900' },
  scoreLg:  { fontSize: 52 },
  badge:    { paddingHorizontal: 10, paddingVertical: 3, borderRadius: 20, borderWidth: 1 },
  label:    { fontSize: 11, fontWeight: '700', letterSpacing: 0.5 },
});
