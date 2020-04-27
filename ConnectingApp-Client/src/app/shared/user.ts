import { Photo } from './photo';

export interface User {
  id: number;
  age: number;
  userName: string;
  knownAs: string;
  gender: string;
  photoUrl: string;
  city: string;
  counntry: string;
  created: Date;
  lastActive: Date;
  interests?: string;
  lookingFor?: string;
  introduction?: string;
  photos?: Photo[];
}
