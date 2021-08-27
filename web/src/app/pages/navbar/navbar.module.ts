import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import lottie from 'lottie-web';
import {IonicModule} from '@ionic/angular';

import {NavbarPageRoutingModule} from './navbar-routing.module';

import {NavbarPage} from './navbar.page';
import {ImagesService} from '../../services/images/images.service';
import {VideosService} from '../../services/videos/videos.service';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {AnalyzedImagesService} from '../../services/analyzed-images/analyzed-images.service';
import {AnalyzedVideosService} from '../../services/analyzed-videos/analyzed-videos.service';
import {NotificationsService} from '../../services/notifications/notifications.service';
import {defineLordIconElement} from 'lord-icon-element';
import {UsersService} from '../../services/users/users.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    NavbarPageRoutingModule
  ],
  declarations: [NavbarPage],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class NavbarPageModule {
  constructor(private imagesService: ImagesService, private videosService: VideosService,
              private pipelineService: PipelineService, private analyzedImagesService: AnalyzedImagesService,
              private analyzedVideosService: AnalyzedVideosService, private notificationsService: NotificationsService,
              private usersService: UsersService) {
    defineLordIconElement(lottie.loadAnimation);

  }
}
