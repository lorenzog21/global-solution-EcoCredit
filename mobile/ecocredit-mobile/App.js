import React, { useState } from 'react';
import { StatusBar } from 'expo-status-bar';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { Ionicons } from '@expo/vector-icons';

import LoginScreen    from './screens/LoginScreen';
import DashboardScreen  from './screens/DashboardScreen';
import FacilitiesScreen from './screens/FacilitiesScreen';
import CreditsScreen    from './screens/CreditsScreen';

const Tab = createBottomTabNavigator();

function MainTabs() {
  return (
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
          if (route.name === 'Dashboard')  iconName = focused ? 'speedometer' : 'speedometer-outline';
          else if (route.name === 'Facilities') iconName = focused ? 'business' : 'business-outline';
          else if (route.name === 'Credits')    iconName = focused ? 'leaf' : 'leaf-outline';
          return <Ionicons name={iconName} size={size} color={color} />;
        },
      })}>
      <Tab.Screen name="Dashboard"  component={DashboardScreen}  options={{ tabBarLabel: 'Dashboard' }} />
      <Tab.Screen name="Facilities" component={FacilitiesScreen} options={{ tabBarLabel: 'Instalações' }} />
      <Tab.Screen name="Credits"    component={CreditsScreen}    options={{ tabBarLabel: 'Créditos' }} />
    </Tab.Navigator>
  );
}

export default function App() {
  const [user, setUser] = useState(null);

  return (
    <>
      <StatusBar style="light" />
      {user ? (
        <NavigationContainer>
          <MainTabs />
        </NavigationContainer>
      ) : (
        <LoginScreen onLogin={setUser} />
      )}
    </>
  );
}
