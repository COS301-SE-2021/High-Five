import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RegisterConstants {
  public labels = {
    'email_address': "Enter Email Address",
    'verify_email': "Verify",
    'enter_code': "Enter Verification Code"
  }
}
