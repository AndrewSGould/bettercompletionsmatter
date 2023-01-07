// TODO: clean up the different uses of the urls..

export const environment = {
  production: false,
  api: {
    domain: 'localhost:4300',
    base: 'http://localhost:4300/',
    baseUrl: 'http://localhost:4300/api/',
    token: 'http://localhost:4300/api/token/refresh'
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
