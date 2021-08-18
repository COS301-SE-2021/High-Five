import {Component, OnInit} from '@angular/core';
import {AnimationOptions} from 'ngx-lottie';
import {ModalController} from '@ionic/angular';
import {MoreInfoComponent} from '../more-info/more-info.component';

@Component({
  selector: 'app-about-page2',
  templateUrl: './about-page2.component.html',
  styleUrls: ['./about-page2.component.scss'],
})
export class AboutPage2Component implements OnInit {

  explanations = [
    {
      title: 'Streaming the live video to multiple clients',
      explanation: '',
    },
    {
      title: 'Flying the drones using our own application',
      explanation: '',
    },
    {
      title: 'Latency Between the drone and backend',
      explanation: '',
    },
    {
      title: 'Another Interesting Design Consideration',
      explanation: '',
    }
  ];

  lottieConfig: AnimationOptions = {
    path: '/assets/lottie-animations/lf30_editor_5gpajdty.json'
  };

  lottieMulticastConfig: AnimationOptions = {
    path: '/assets/lottie-animations/multicast.json',
  };

  lottieMobileAppConfig: AnimationOptions = {
    path: '/assets/lottie-animations/72680-mobile-app.json',
  };

  lottieLatencyConfig: AnimationOptions = {
    path: '/assets/lottie-animations/speed.json',
  };

  constructor(private modalController: ModalController) {
  }

  ngOnInit() {
  }


  async displayModal(choice: number) {
    const modal = await this.modalController.create({
      component: MoreInfoComponent,
      showBackdrop: true,
      animated: true,
      backdropDismiss: true,
      componentProps: {
        title: this.explanations[choice].title,
        description: this.explanations[choice].explanation,
      }
    });
    await modal.present();
  }

}
