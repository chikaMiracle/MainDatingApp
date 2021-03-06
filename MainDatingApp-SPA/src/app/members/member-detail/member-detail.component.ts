import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap';
// import * as moment from 'moment'; // import moment.

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, AfterViewInit {

  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  @ViewChild('memberTabs',{static:false}) memberTabs: TabsetComponent

  constructor(private userService: UserService, 
    private alertify: AlertifyService, private route: ActivatedRoute) {
      //this.lastUpdated = this.getCurrentTime(); **// this shows error.. that type string is not assignable to type 'Date'**
     }

  ngOnInit() {
   this.route.data.subscribe(data => {
     this.user = data['user'];
   });

   


   this.galleryOptions = [
    {
       width: '500px',
       height: '500px',
       imagePercent: 100,
       thumbnailsColumns: 4,
       imageAnimation: NgxGalleryAnimation.Slide,
       preview: false
    }
  ];

   this.galleryImages = this.getImages();
}

ngAfterViewInit(){
  this.route.queryParams.subscribe(params => {
    const selectedTab = params['tab'];
    this.memberTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
  });
}


// var getCurrentTime(){
//   return moment().format('DD MMM YYYY HH:mm:ss'); 
// }
//Array of images

  getImages(){
    const imageUrls = [];
    //to loop over the users image we get back
    for(let i=0; i<this.user.photos.length; i++){

      //to add the users image to an array we use push
      imageUrls.push({
             small: this.user.photos[i].url,
             medium: this.user.photos[i].url,
             big: this.user.photos[i].url,
             description: this.user.photos[i].description
      })
    }
     return imageUrls;
  }
   

  selectTab(tabId: number){
    this.memberTabs.tabs[tabId].active = true;
  }

  // loadUser(){
  //   this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user: User) =>{
  //     this.user = user;
  //   }, error =>{
  //     this.alertify.error(error);
  //   })
  // }

}
