export class PipelineModel{
  private name: string;
  private id: number;
  private selected: boolean[];

  constructor(name: string, selectedTools: boolean[]) {
    this.name=  name;
    this.setSelectedTools(selectedTools);
  }

  get selectedTools(): boolean[]{
    return this.selected;
  }

  setSelectedTools(selectedTools: boolean[]){
    delete this.selected;
    this.selected = new Array<boolean>(selectedTools.length);
    for (let i = 0; i < selectedTools.length; i++) {
      this.selected[i] = selectedTools[i];
    }

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
