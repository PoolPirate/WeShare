import { Injectable } from "@angular/core";
import { ProfileInfo } from "../../../../types/profile-types";
import { UserSnippet } from "../../../../types/user-types";

@Injectable()
export class ProfileStore {
  userSnippet: UserSnippet;
  profileInfo: ProfileInfo;
}
