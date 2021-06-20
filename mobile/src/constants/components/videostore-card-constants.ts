import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})

/**
 * Contains default labels that are used in the program.
 */
export class VideoStoreConstants {

  /**
   * These labels are for the Toast controller object when it displays messages
   */
  public toastLabels = {
    header: 'Video Uploaded',
    message: 'Video successfully uploaded.',
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
