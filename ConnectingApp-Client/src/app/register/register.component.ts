import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_service/auth.service';
import { AlertifyService } from '../_service/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/ngx-bootstrap-datepicker';
import { User } from '../shared/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../shared/site.css']
})
export class RegisterComponent implements OnInit {

  // for the datepicker
  // to make all the properties in type are optional
  // bsdatepicker is the type and all the props inside it are optional now
  bsConfig: Partial<BsDatepickerConfig>;

  // for reactive forms
  // form group will keep the values and validity of form control instances
  // we create the register form in ngOnInit 
  registerForm: FormGroup;

  // to get the values from parent component
  @Input() valuesFromParent: any;

  // to pass data to parent component
  @Output() cancelRegistration = new EventEmitter();
  user: User;
  constructor(private service: AuthService, private alertify: AlertifyService,
    private fb: FormBuilder, // to create form with formbuilder
    private route: Router
  ) { }

  ngOnInit(): void {
    // to set theme of datepicker
    this.bsConfig = {
      containerClass: 'theme-dark-blue'
    },


      //// here we create the register form
      //// we pass 'registerForm' into <form [formGroup]="here"
      //this.registerForm = new FormGroup({
      //  // to add validations we pass parameters inside form control
      //  username: new FormControl('', Validators.required),
      //  password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      //  confirmPassword: new FormControl('', Validators.required)

      //},
      //  // we'll call the custom validator here
      //  this.passwordMatchValidator
      //);

      // now we'll call the create method her
      this.CreateForm();
  }

  // we can user formbuilder to create our form as well
  CreateForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      counntry: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, // to pass custom validator in her we use key valye pair
      { validator: this.passwordMatchValidator });
  }

  // to match the password we have to do custom validator
  passwordMatchValidator(f: FormGroup) {
    return f.get('password').value === f.get('confirmPassword').value ? null : { 'mismatch': true };
  }

  register() {
    // here we call the register method from service and also subscribe it
    //this.service.register(this.model).subscribe(() => {
    //  this.alertify.success("registered successfully");
    //},
    //  error => {
    //    this.alertify.error(error);
    //  });

    // now we register here using form values
    if (this.registerForm.valid) {
      // we are setting the values of form to user
      // 1st parameter is emty object that is being return by this func
      this.user = Object.assign({}, this.registerForm.value);
      this.service.register(this.user).subscribe(() => {
        this.alertify.success("Registration Successful!");
      }, error => {
          this.alertify.error(error);
      }, // the 3rd parameter is what to on completion of this method so we login the registered user
        () => {
          this.service.login(this.user).subscribe(() => {
            // after login we sent the newly registered user to members page
            this.route.navigate(['/members']);
          });
      });
    }
    console.log(this.registerForm.value);
  }

  cancel() {
    // to send value to parent component we emit values
    // inside emit we can send any data for our case we just send false value
    this.cancelRegistration.emit(false);
  }

}
