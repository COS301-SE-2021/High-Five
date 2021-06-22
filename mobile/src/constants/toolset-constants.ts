import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})

/**
 * Contains default labels that are used in the program.
 */
export class ToolsetConstants {
  public labels = {
    tools: ['Object Identification','Object Counting', 'Object Tracking'],
    newPipeline : ['New Pipeline Name']
  };
}
