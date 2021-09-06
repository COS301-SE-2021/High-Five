import {MetaData} from './metaData';
export interface UserTool {
  onnxModel?: any;
  id: string;
  name: string;
  toolClassFile?: any;
  type: string;
  metaDataType?: MetaData;
}
