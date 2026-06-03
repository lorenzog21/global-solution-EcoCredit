import React from 'react';
import { StatusBar } from 'expo-status-bar';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { Ionicons } from '@expo/vector-icons';

import DashboardScreen from './screens/DashboardScreen';
import FacilitiesScreen from './screens/FacilitiesScreen';
import CreditsScreen from './screens/CreditsScreen';

const Tab = createBottomTabNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <StatusBar style="light" />
      <Tab.Navigator
        screenOptions={({ route }) => ({
          headerShown: false,
          tabBarStyle: {
            backgroundColor: '#0b110e',
            borderTopColor: '#1a2e22',
            paddingBottom: 8,
            height: 70,
          },
          tabBarActiveTintColor: '#00e87a',
          tabBarInactiveTintColor: '#5a7a60',
          tabBarIcon: ({ focused, color, size }) => {
            let iconName;
            if (route.name === 'Dashboard') iconName = focused ? 'speedometer' : 'speedometer-outline';
            else if (route.name === 'Facilities') iconName = focused ? 'business' : 'business-outline';
            else if (route.name === 'Credits') iconName = focused ? 'leaf' : 'leaf-outline';
            return <Ionicons name={iconName} size={size} color={color} />;
          },
        })}>
        <Tab.Screen name="Dashboard" component={DashboardScreen} options={{ tabBarLabel: 'Dashboard' }} />
        <Tab.Screen name="Facilities" component={FacilitiesScreen} options={{ tabBarLabel: 'Instalações' }} />
        <Tab.Screen name="Credits" component={CreditsScreen} options={{ tabBarLabel: 'Créditos' }} />
      </Tab.Navigator>
    </NavigationContainer>
  );
}
