import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../shared/user';
import { AlertifyService } from '../../_service/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from '../../_service/user.service';
import { AuthService } from '../../_service/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['../../shared/site.css']
})
export class MemberEditComponent implements OnInit {

  // to prevent user to close the tab before saving the changes we use @HostListener decorator
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  // to access the form in html we use this decorator
  @ViewChild('editForm', { static: true }) editForm: NgForm;
  user: User;
  constructor(private route: ActivatedRoute,
    private alertify: AlertifyService,
    private userService: UserService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  UpdateUser() {
    // decodedtoken.nameid contains the id of loggedin user
    this.userService.UpdateUser(this.authService.decodedToken.nameid, this.user)
      .subscribe(next => {
        this.alertify.success("Profile updated successfully!");

        // to reset the savechanges button and prompt text
        // if we don't pass the user here then all user data reset to empty strings
        this.editForm.resetForm(this.user);
      },
        error => {
          this.alertify.error(error);
        });
    
  }

}
