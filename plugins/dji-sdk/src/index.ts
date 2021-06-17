import { registerPlugin } from '@capacitor/core';

import type { DjiSdkPlugin } from './definitions';

const DjiSdk = registerPlugin<DjiSdkPlugin>('DjiSdk', {
  web: () => import('./web').then(m => new m.DjiSdkWeb()),
});

export * from './definitions';
export { DjiSdk };
