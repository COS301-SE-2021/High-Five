import {Component, Input, OnInit} from '@angular/core';
import {AnalyzedImageMetaData} from '../../models/analyzedImageMetaData';
import {AnalyzedImagesService} from '../../services/analyzed-images/analyzed-images.service';
import {AlertController} from '@ionic/angular';

@Component({
  selector: 'app-analyzed-image-card',
  templateUrl: './analyzed-image-card.component.html',
  styleUrls: ['./analyzed-image-card.component.scss'],
})
export class AnalyzedImageCardComponent implements OnInit {

  /**
   * The analyzed image which this component will represent
   */
  @Input() analyzedImage: AnalyzedImageMetaData;

  constructor(private analyzedImagesService: AnalyzedImagesService, private alertController: AlertController) {
  }

  ngOnInit() {
  }

  /**
   * Opens a new tab, to enlarge the image, this way the user can view the image fullscreen
   */
  public async viewImageFullScreen() {
    const newWindow = window.open(this.analyzedImage.url, '_system');
    newWindow.focus();
  }

  public async onDeleteAnalyzedImage() {
    const alert = await this.alertController.create({
      header: 'Analysed Image Deletion',
      message: `Are you sure you want to delete this analysed image ?`,
      animated: true,
      translucent: true,
      buttons: [
        {
          text: 'Cancel',
          handler: () => {
          }
        }, {
          text: `I'm Sure`,
          handler: () => {
            this.analyzedImagesService.deleteAnalyzedImage(this.analyzedImage.id);
          }
        }
      ]
    });

    await alert.present();
  }
}
