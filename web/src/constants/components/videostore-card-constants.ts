import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})

/**
 * Contains default labels that are used in the program.
 */
export class VideoStoreCardConstants {

  /**
   * These labels are for the Toast controller object when it displays messages
   */
  public toastLabels = {
    header: 'Video Deleted',
    message: 'Video successfully deleted.',
    buttons: ['OK']
  };

  /**
   * These labels are for the Alert controller object when it displays messages
   */
  public alertLabels = {
    header: 'Delete video',
    message: 'Do you want to delete the video?',
    buttonsYes:
      {
        text: 'Yes',
        role: 'yes'
      },
    buttonsNo:
      {
        text: 'No',
        role: 'no'
      }
  };
}
