import { Injectable } from "@angular/core";
import { ShareData, ShareUserData } from "../../../../types/share-types";


@Injectable()
export class ShareService {
  shareData: ShareData;
  shareUserData: ShareUserData | null;
}
