import React from 'react';
import { View, Text, ScrollView, StyleSheet, TouchableOpacity } from 'react-native';
import { MOCK_COMPANY, MOCK_FACILITIES, MOCK_ALERTS } from '../data/mockData';

const STATUS_COLORS = {
  NORMAL:   '#00e87a',
  WARNING:  '#ffaa00',
  CRITICAL: '#ff4d6a',
};

const SEVERITY_COLORS = {
  CRITICAL: '#ff4d6a',
  HIGH:     '#ffaa00',
  MEDIUM:   '#4da6ff',
};

export default function DashboardScreen({ navigation }) {
  const criticalCount = MOCK_ALERTS.filter(a => a.severity === 'CRITICAL').length;

  return (
    <ScrollView style={styles.container} showsVerticalScrollIndicator={false}>
      <View style={styles.header}>
        <Text style={styles.headerSub}>EcoCredit · ESG Dashboard</Text>
        <Text style={styles.headerCompany}>{MOCK_COMPANY.name}</Text>
      </View>

      <View style={styles.scoreCard}>
        <Text style={styles.scoreLabel}>Compliance Score</Text>
        <Text style={[styles.scoreValue,
          { color: MOCK_COMPANY.complianceScore >= 80 ? '#00e87a'
                  : MOCK_COMPANY.complianceScore >= 60 ? '#ffaa00'
                  : '#ff4d6a' }]}>
          {MOCK_COMPANY.complianceScore}%
        </Text>
        <Text style={styles.scoreDesc}>
          {MOCK_COMPANY.complianceScore >= 80 ? '✅ Em conformidade ESG'
          : MOCK_COMPANY.complianceScore >= 60 ? '⚠️ Atenção necessária'
          : '🚨 Fora de conformidade'}
        </Text>
      </View>

      <View style={styles.kpiRow}>
        <View style={styles.kpiCard}>
          <Text style={styles.kpiValue}>{MOCK_COMPANY.creditBalance.toLocaleString()}</Text>
          <Text style={styles.kpiLabel}>tCO₂ Créditos</Text>
        </View>
        <View style={[styles.kpiCard, { borderColor: criticalCount > 0 ? '#ff4d6a' : '#1a2e22' }]}>
          <Text style={[styles.kpiValue, { color: criticalCount > 0 ? '#ff4d6a' : '#00e87a' }]}>
            {criticalCount}
          </Text>
          <Text style={styles.kpiLabel}>Alertas Críticos</Text>
        </View>
        <View style={styles.kpiCard}>
          <Text style={styles.kpiValue}>{MOCK_FACILITIES.length}</Text>
          <Text style={styles.kpiLabel}>Instalações</Text>
        </View>
      </View>

      <Text style={styles.sectionTitle}>Instalações</Text>
      {MOCK_FACILITIES.map(f => (
        <TouchableOpacity
          key={f.facilityId}
          style={styles.facilityCard}
          onPress={() => navigation.navigate('Facilities')}>
          <View style={styles.facilityHeader}>
            <Text style={styles.facilityName}>{f.name}</Text>
            <View style={[styles.statusBadge, { backgroundColor: STATUS_COLORS[f.status] + '30' }]}>
              <Text style={[styles.statusText, { color: STATUS_COLORS[f.status] }]}>{f.status}</Text>
            </View>
          </View>
          <View style={styles.barTrack}>
            <View style={[styles.barFill, {
              width: `${Math.min(f.percentUsed, 100)}%`,
              backgroundColor: STATUS_COLORS[f.status]
            }]} />
          </View>
          <Text style={styles.facilityDetail}>
            {f.emittedTco2e.toLocaleString()} / {f.limitTco2.toLocaleString()} tCO₂e ({f.percentUsed}%)
          </Text>
        </TouchableOpacity>
      ))}

      <Text style={styles.sectionTitle}>Alertas Recentes</Text>
      {MOCK_ALERTS.slice(0, 3).map(a => (
        <View key={a.alertId} style={[styles.alertCard, { borderLeftColor: SEVERITY_COLORS[a.severity] }]}>
          <Text style={[styles.alertSeverity, { color: SEVERITY_COLORS[a.severity] }]}>{a.severity}</Text>
          <Text style={styles.alertMsg}>{a.message}</Text>
          <Text style={styles.alertMeta}>{a.facility} · {a.time}</Text>
        </View>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container:      { flex: 1, backgroundColor: '#060a08' },
  header:         { padding: 24, paddingTop: 60, paddingBottom: 16 },
  headerSub:      { fontSize: 11, color: '#5a7a60', letterSpacing: 2, textTransform: 'uppercase', marginBottom: 4 },
  headerCompany:  { fontSize: 22, fontWeight: '800', color: '#ffffff' },
  scoreCard:      { margin: 16, padding: 24, backgroundColor: '#0b110e', borderRadius: 16, borderWidth: 1, borderColor: '#1a2e22', alignItems: 'center' },
  scoreLabel:     { fontSize: 12, color: '#5a7a60', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 8 },
  scoreValue:     { fontSize: 52, fontWeight: '900' },
  scoreDesc:      { fontSize: 13, color: '#ddeedd', marginTop: 8 },
  kpiRow:         { flexDirection: 'row', gap: 10, marginHorizontal: 16, marginBottom: 16 },
  kpiCard:        { flex: 1, backgroundColor: '#0b110e', borderRadius: 12, padding: 14, borderWidth: 1, borderColor: '#1a2e22', alignItems: 'center' },
  kpiValue:       { fontSize: 22, fontWeight: '900', color: '#00e87a' },
  kpiLabel:       { fontSize: 10, color: '#5a7a60', textTransform: 'uppercase', marginTop: 4, textAlign: 'center' },
  sectionTitle:   { fontSize: 13, fontWeight: '700', color: '#00e87a', marginHorizontal: 16, marginBottom: 10, textTransform: 'uppercase', letterSpacing: 2 },
  facilityCard:   { marginHorizontal: 16, marginBottom: 10, backgroundColor: '#0b110e', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#1a2e22' },
  facilityHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 },
  facilityName:   { fontSize: 14, fontWeight: '700', color: '#ffffff' },
  statusBadge:    { paddingHorizontal: 8, paddingVertical: 2, borderRadius: 20 },
  statusText:     { fontSize: 10, fontWeight: '700' },
  barTrack:       { height: 6, backgroundColor: '#1a2e22', borderRadius: 3, marginBottom: 8, overflow: 'hidden' },
  barFill:        { height: '100%', borderRadius: 3 },
  facilityDetail: { fontSize: 11, color: '#5a7a60' },
  alertCard:      { marginHorizontal: 16, marginBottom: 8, backgroundColor: '#0b110e', borderRadius: 10, padding: 14, borderWidth: 1, borderColor: '#1a2e22', borderLeftWidth: 3 },
  alertSeverity:  { fontSize: 10, fontWeight: '700', letterSpacing: 1, marginBottom: 4 },
  alertMsg:       { fontSize: 13, color: '#ddeedd', marginBottom: 4 },
  alertMeta:      { fontSize: 11, color: '#5a7a60' },
});
