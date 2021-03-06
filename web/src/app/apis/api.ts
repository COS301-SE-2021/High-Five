export * from './analysis.service';
import {AnalysisService} from './analysis.service';

export * from './downloads.service';
import {DownloadsService} from './downloads.service';

export * from './livestream.service';
import {LivestreamService} from './livestream.service';

export * from './mediaStorage.service';
import {MediaStorageService} from './mediaStorage.service';

export * from './pipelines.service';
import {PipelinesService} from './pipelines.service';

export * from './test.service';
import {TestService} from './test.service';

export * from './tools.service';
import {ToolsService} from './tools.service';

export * from './user.service';
import {UserService} from './user.service';

export const APIS = [AnalysisService, DownloadsService, LivestreamService, MediaStorageService, PipelinesService, TestService, ToolsService, UserService];
