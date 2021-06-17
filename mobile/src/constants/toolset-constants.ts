import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})

/**
 * Contains default labels that are used in the program.
 */
export class ToolsetConstants {
  public labels = {
    tool1: 'Object Identification',
    tool2: 'Object Counting',
    tool3 : 'Object Tracking',
    tool4 : 'Some other thing',
  };
}
