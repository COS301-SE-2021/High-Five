export interface DjiSdkPluginPlugin {
  echo(options: { value: string }): Promise<{ value: string }>;
}
