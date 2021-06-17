import { WebPlugin } from '@capacitor/core';

import type { DjiSdkPlugin } from './definitions';

export class DjiSdkWeb extends WebPlugin implements DjiSdkPlugin {
  constructor() {
    super({
      name: "DjiSdk",
      platforms: ["web"],
    });
  }
  present(options: { message: string }): void {
    console.log("present", options);
  }

  async echo(options: { value: string }): Promise<{ value: string }> {
    console.log('ECHO', options);
    return options;
  }
}
