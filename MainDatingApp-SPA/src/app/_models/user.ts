import { PathLocationStrategy } from '@angular/common';
import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    knownAs:string;
    age:number;
    gender:string;
    created: Date;
    lastAtive: Date;
    photoUrl:string;
    city:string;
    country:string;
    interests?:string;
    introduction?:string;
    lookingFor?:string;
    photos?:Photo[];
    
}
