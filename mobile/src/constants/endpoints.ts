import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',

})

/**
 * Contains default labels that are used in the program.
 */
export class Endpoints {
  public labels = {
    getAllVideos: 'https://high5api.azurewebsites.net/media/getAllVideos',
    storeVideo: 'https://high5api.azurewebsites.net/media/storeVideo',
    getVideo: 'https://high5api.azurewebsites.net/media/getVideo',
    deleteVideo: 'https://high5api.azurewebsites.net/media/deleteVideo'
  };
}
