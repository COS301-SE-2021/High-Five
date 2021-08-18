export * from './analysis.service';
import { AnalysisService } from './analysis.service';
export * from './mediaStorage.service';
import { MediaStorageService } from './mediaStorage.service';
export * from './pipelines.service';
import { PipelinesService } from './pipelines.service';
export * from './test.service';
import { TestService } from './test.service';
export const APIS = [AnalysisService, MediaStorageService, PipelinesService, TestService];
