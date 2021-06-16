import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})

/**
 * Contains default labels that are used in the program.
 */
export class VideoUploadConstants {
  public labels = {
    'file_name': "File Name",
    'file_upload': "Upload"
  }
}
