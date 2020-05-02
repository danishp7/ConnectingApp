import { Component, OnInit, Input } from '@angular/core';
import { User } from '../../shared/user';
import { AuthService } from '../../_service/auth.service';
import { AlertifyService } from '../../_service/alertify.service';
import { UserService } from '../../_service/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['../../shared/site.css']
})
export class MemberCardComponent implements OnInit {

  // to get the input from parent (member-list) component
  @Input() userFromMemberList: User;

  // we need these services to like the user functionality
  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private userService: UserService

  ) { }


  ngOnInit(): void {
  }

  sendLike(recipientId: number) {

    // 1st arg: to get id of currently logged in user
    // 2nd arg: id of other user to be liked
    this.userService.sendLike(this.authService.decodedToken.nameid, recipientId)
      .subscribe(response => {
        this.alertify.success("You liked " + this.userFromMemberList.userName);
      },
        error => {
          this.alertify.error("You have already liked " + this.userFromMemberList.userName);
        });
  }

}
