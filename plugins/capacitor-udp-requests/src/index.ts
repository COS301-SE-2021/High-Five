import { registerPlugin } from '@capacitor/core';

import type { UdpRequestsPlugin } from './definitions';

const UdpRequests = registerPlugin<UdpRequestsPlugin>('UdpRequests', {
  web: () => import('./web').then(m => new m.UdpRequestsWeb()),
});

export * from './definitions';
export { UdpRequests };
