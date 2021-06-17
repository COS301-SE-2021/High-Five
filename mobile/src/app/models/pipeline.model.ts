export class PipelineModel{
  private name: string;
  private id: number;
  private selected: boolean[];

  constructor(name: string) {
    this.name=  name;
  }

  get selectedTools(): boolean[]{
    return this.selected;
  }

  set selectedTools(selectedTools: boolean[]){
    this.selected=selectedTools;

  }

  get pipelineName(): string {
    return this.name;
  }

  set pipelineName(value: string) {
    this.name = value;
  }

  get pipelineId(): number {
    return this.id;
  }

  set pipelineId(value: number) {
    this.id = value;
  }
}
