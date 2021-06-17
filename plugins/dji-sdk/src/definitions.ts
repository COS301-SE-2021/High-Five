

export interface DjiSdkPlugin {
  echo(options: { value: string }): Promise<{ value: string }>;
  present(options: { message: string }): void;
}
