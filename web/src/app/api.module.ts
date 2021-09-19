import {NgModule, ModuleWithProviders, SkipSelf, Optional} from '@angular/core';
import {Configuration} from './configuration';
import {HttpClient} from '@angular/common/http';


import {AnalysisService} from './apis/analysis.service';
import {DownloadsService} from './apis/downloads.service';
import {MediaStorageService} from './apis/mediaStorage.service';
import {PipelinesService} from './apis/pipelines.service';
import {TestService} from './apis/test.service';
import {ToolsService} from './apis/tools.service';
import {UserService} from './apis/user.service';

@NgModule({
  imports: [],
  declarations: [],
  exports: [],
  providers: [
    AnalysisService,
    DownloadsService,
    MediaStorageService,
    PipelinesService,
    TestService,
    ToolsService,
    UserService]
})
export class ApiModule {

  constructor(@Optional() @SkipSelf() parentModule: ApiModule,
              @Optional() http: HttpClient) {
    if (parentModule) {
      throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
    }
    if (!http) {
      throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
        'See also https://github.com/angular/angular/issues/20575');
    }
  }

  public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders<any> {
    return {
      ngModule: ApiModule,
      providers: [{provide: Configuration, useFactory: configurationFactory}]
    };
  }

}
