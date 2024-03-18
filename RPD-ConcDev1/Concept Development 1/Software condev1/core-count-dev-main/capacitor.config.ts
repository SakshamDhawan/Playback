import { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'com.solder.corecountdev',
  appName: 'Core Count Dev',
  webDir: 'dist',
  server: {
    androidScheme: 'https'
  }
};

export default config;
