import { Component, OnInit, Input } from '@angular/core';
import { MessageService } from '../../_service/message.service';
import { AlertifyService } from '../../_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../_service/auth.service';
import { Message } from '../../shared/message';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['../../shared/site.css']
})
export class MemberMessagesComponent implements OnInit {

  // this will be the child component of detail component
  // id will be passed by detail component to this
  // this id will be recipientid
  // sender id will get using token
  @Input() recipientId: number;
  messages: Message[];

  // to send new message
  newMessage: any = {};

  constructor(private messageService: MessageService,
    private alertify: AlertifyService, private route: ActivatedRoute, private authService: AuthService) { }

  ngOnInit(): void {

    this.loadMessages();
  }

  loadMessages() {
    this.messageService.getConversation(this.authService.decodedToken.nameid, this.recipientId)
      // to set the mark as read
      .pipe(
        tap(messages => {
          for (let i = 0; i < messages.length; i++) {
            if (messages[i].isRead === false && messages[i].recipientId == this.authService.decodedToken.nameid) {
              this.messageService.markAsRead(this.authService.decodedToken.nameid, messages[i].id)
            }
          }
        })
      )
      .subscribe(messages => {
        this.messages = messages;
      }, error => {
          this.alertify.error(error);
      });
  }

  // to send the message
  sendMessage() {
    // first set the recipientId
    this.newMessage.recipientId = this.recipientId;

    // now call the service and subscribe it
    // sender id would be in token
    this.messageService.sendMessage(this.authService.decodedToken.nameid, this.newMessage)
      .subscribe((message: Message) => {

        // to add the message at start of the array
        this.messages.unshift(message);

        // now reset the content in input
        this.newMessage.content = '';
      }, error => {
          this.alertify.error(error);
      });
  }

}
