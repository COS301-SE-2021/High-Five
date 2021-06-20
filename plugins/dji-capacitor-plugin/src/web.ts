import { WebPlugin } from '@capacitor/core';

import type { DjiSdkPluginPlugin } from './definitions';

export class DjiSdkPluginWeb extends WebPlugin implements DjiSdkPluginPlugin {
  async echo(options: { value: string }): Promise<{ value: string }> {
    console.log('ECHO', options);
    return options;
  }
}
