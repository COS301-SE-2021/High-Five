import { WebPlugin } from '@capacitor/core';

import type { DjiSdkPlugin } from './definitions';

export class DjiSdkWeb extends WebPlugin implements DjiSdkPlugin {
  async echo(options: { value: string }): Promise<{ value: string }> {
    console.log('ECHO', options);
    return options;
  }
}
