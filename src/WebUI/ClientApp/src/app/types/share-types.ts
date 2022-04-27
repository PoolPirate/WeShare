import { UserSnippet } from "./user-types";

export class ShareSnippet {
  id: number;
  name: string;
  description: string;
  ownerUsername: string;
}

export class ShareSecrets {
  bodyProcessingType: number;
  headerProcessingType: number;
  secret: string;
}

export class ShareData {
  shareInfo: ShareInfo;
  ownerSnippet: UserSnippet;
}

export class ShareUserData {
  liked: boolean;
}

export class ShareInfo {
  id: number;
  createdAt: Date;
  likeCount: number;
  subscriberCount: number;
  name: string;
  description: string;
  readme: string;
  ownerId: number;
}
