import { registerPlugin } from '@capacitor/core';

import type { DjiSdkPluginPlugin } from './definitions';

const DjiSdkPlugin = registerPlugin<DjiSdkPluginPlugin>('DjiSdkPlugin', {
  web: () => import('./web').then(m => new m.DjiSdkPluginWeb()),
});

export * from './definitions';
export { DjiSdkPlugin };
