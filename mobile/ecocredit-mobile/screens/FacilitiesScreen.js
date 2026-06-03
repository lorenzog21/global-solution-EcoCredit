import React from 'react';
import { View, Text, ScrollView, StyleSheet } from 'react-native';
import { MOCK_FACILITIES } from '../data/mockData';

const STATUS_COLORS = {
  NORMAL:   '#00e87a',
  WARNING:  '#ffaa00',
  CRITICAL: '#ff4d6a',
};

const FACILITY_ICONS = {
  REFINERY: '🏭',
  RIG:      '⚓',
  PLANT:    '🔧',
  FARM:     '🌾',
  MINE:     '⛏️',
  FLEET:    '🚛',
};

export default function FacilitiesScreen() {
  return (
    <ScrollView style={styles.container} showsVerticalScrollIndicator={false}>
      <View style={styles.header}>
        <Text style={styles.headerSub}>EcoCredit · Instalações</Text>
        <Text style={styles.headerTitle}>Monitoramento de Emissões</Text>
      </View>

      {MOCK_FACILITIES.map(f => (
        <View key={f.facilityId} style={[styles.card, { borderTopColor: STATUS_COLORS[f.status] }]}>
          <View style={styles.cardHeader}>
            <Text style={styles.icon}>{FACILITY_ICONS[f.type] || '🏗️'}</Text>
            <View style={styles.cardTitles}>
              <Text style={styles.facilityName}>{f.name}</Text>
              <Text style={styles.facilityType}>{f.type}</Text>
            </View>
            <View style={[styles.badge, { backgroundColor: STATUS_COLORS[f.status] + '25' }]}>
              <Text style={[styles.badgeText, { color: STATUS_COLORS[f.status] }]}>{f.status}</Text>
            </View>
          </View>

          <View style={styles.metrics}>
            <View style={styles.metric}>
              <Text style={styles.metricValue}>{f.emittedTco2e.toLocaleString()}</Text>
              <Text style={styles.metricLabel}>tCO₂e emitido</Text>
            </View>
            <View style={styles.metric}>
              <Text style={styles.metricValue}>{f.limitTco2.toLocaleString()}</Text>
              <Text style={styles.metricLabel}>Limite mensal</Text>
            </View>
            <View style={styles.metric}>
              <Text style={[styles.metricValue, { color: STATUS_COLORS[f.status] }]}>{f.percentUsed}%</Text>
              <Text style={styles.metricLabel}>Utilizado</Text>
            </View>
          </View>

          <View style={styles.barTrack}>
            <View style={[styles.barFill, {
              width: `${Math.min(f.percentUsed, 100)}%`,
              backgroundColor: STATUS_COLORS[f.status]
            }]} />
          </View>

          <Text style={styles.lastReading}>
            Última leitura: {new Date(f.lastReading).toLocaleString('pt-BR')}
          </Text>
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
  card:          { marginHorizontal: 16, marginBottom: 12, backgroundColor: '#0b110e', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#1a2e22', borderTopWidth: 3 },
  cardHeader:    { flexDirection: 'row', alignItems: 'center', marginBottom: 16, gap: 10 },
  icon:          { fontSize: 24 },
  cardTitles:    { flex: 1 },
  facilityName:  { fontSize: 15, fontWeight: '700', color: '#ffffff' },
  facilityType:  { fontSize: 11, color: '#5a7a60', marginTop: 2 },
  badge:         { paddingHorizontal: 10, paddingVertical: 4, borderRadius: 20 },
  badgeText:     { fontSize: 10, fontWeight: '700' },
  metrics:       { flexDirection: 'row', justifyContent: 'space-between', marginBottom: 12 },
  metric:        { alignItems: 'center' },
  metricValue:   { fontSize: 18, fontWeight: '900', color: '#ffffff' },
  metricLabel:   { fontSize: 10, color: '#5a7a60', marginTop: 2 },
  barTrack:      { height: 8, backgroundColor: '#1a2e22', borderRadius: 4, marginBottom: 10, overflow: 'hidden' },
  barFill:       { height: '100%', borderRadius: 4 },
  lastReading:   { fontSize: 11, color: '#5a7a60' },
});
