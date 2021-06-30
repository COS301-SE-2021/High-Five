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
}
