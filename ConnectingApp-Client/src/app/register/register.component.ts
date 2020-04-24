import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_service/auth.service';
import { AlertifyService } from '../_service/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  // to get the values from parent component
  @Input() valuesFromParent: any;

  // to pass data to parent component
  @Output() cancelRegistration = new EventEmitter();
  model: any = {};
  constructor(private service: AuthService, private alertify: AlertifyService) { }

  ngOnInit(): void {
  }

  register() {
    // here we call the register method from service and also subscribe it
    this.service.register(this.model).subscribe(() => {
      this.alertify.success("registered successfully");
    },
      error => {
        this.alertify.error(error);
      });
  }

  cancel() {
    // to send value to parent component we emit values
    // inside emit we can send any data for our case we just send false value
    this.cancelRegistration.emit(false);
  }

}
