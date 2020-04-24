import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs'

@Injectable({
  providedIn: "root"
})
export class AlertifyService {
  confirm(message: string, okCallBack: () => any) {
    alertify.confirm(message, (e: any) => {
      if (e) {
        okCallBack();
      } else {

      }
    });
  }

  success(message: string) {
    alertify.success(message);
  }

  error(error: string) {
    alertify.error(error);
  }

  warning(warning: string) {
    alertify.warning(warning);
  }

  message(message: string) {
    alertify.message(message);
  }
}
