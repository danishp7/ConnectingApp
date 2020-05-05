import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../shared/user';
import { UserService } from '../../_service/user.service';
import { AlertifyService } from '../../_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryImage, NgxGalleryOptions, NgxGalleryAnimation } from 'ngx-gallery-9';
import { TabsetComponent } from 'ngx-bootstrap/tabs/public_api';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['../../shared/site.css']
})
export class MemberDetailComponent implements OnInit {

  // to set the tab id
  @ViewChild('tabs', { static: true }) memberTabs: TabsetComponent;

  // to use the gallery options and we set these values in ngOnInit
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  user: User;
  // we need user, alertify service and Activated routes to get the id of user
  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  // now we are not using loadUser method instead we read the route and get the data from from resolver
  ngOnInit(): void {
    this.route.data.subscribe(data => {
      // user specified in data must be match with
      // resolve: { user: MemberDetailResolver } in path
      this.user = data['user']; 
    });

    // after clicking the message, to directly go to message tab
    this.route.queryParams.subscribe(params => {
      const selectedTab = params['tab'];
      this.memberTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
    });

    // to set the photo gallery
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 5,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: true
      }
    ];
    this.galleryImages = this.getImages();
  }

  // to get all the images
  getImages() {
    const imgUrls = [];
    for (const photo of this.user.photos) {
      imgUrls.push({ // these are ngx gallery properties
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description
      });
    }

    return imgUrls;
  }

  // to set the id of tabs
  // we call this method on message button
  selectTab(id: number) {
    this.memberTabs.tabs[id].active = true;
  }

  
  //loadUser() {
  //  return this.userService.getUser(+this.route.snapshot.paramMap.get('id'))
  //    .subscribe((response: User) => {
  //      this.user = response;
  //    }, error => {
  //        this.alertify.error(error);
  //    });
  //}

}
