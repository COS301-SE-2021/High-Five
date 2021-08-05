// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  clientId: '315e72e5-4305-47d2-bd7b-406155e610d0',
  authorityId: '607eec4d-ca5f-417d-a9d3-6f794248e805',
  redirectUri: 'http://localhost:8100/navbar/landing',
  b2cPolicies: {
    names: {
      signUpSignIn: 'B2C_1_signupsignin1',
      editProfile: 'B2C_1_profileediting1'
    },
    authorities: {
      signUpSignIn: {
        // authority: 'https://fabrikamb2c.b2clogin.com/fabrikamb2c.onmicrosoft.com/b2c_1_susi',
        authority: 'https://highfiveactivedirectory.b2clogin.com/highfiveactivedirectory.onmicrosoft.com/B2C_1_signupsignin1',
      },
      editProfile: {
        authority: 'https://highfiveactivedirectory.b2clogin.com/highfiveactivedirectory.onmicrosoft.com/B2C_1_profileediting1'
      }
    },
    authorityDomain: 'highfiveactivedirectory.b2clogin.com'
  }
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
