import { UserSnippet } from "./user-types";

export class ShareSnippet {
  id: number;
  name: string;
  isPrivate: boolean;
  description: string;
  ownerUsername: string;
}

export class ShareSecrets {
  payloadProcessingType: number;
  headerProcessingType: number;
  secret: string;
}

export class ShareData {
  shareInfo: ShareInfo;
  ownerSnippet: UserSnippet;
}

export class ShareUserData {
  liked: boolean;
  subscribed: boolean;
}

export class ShareInfo {
  id: number;
  createdAt: Date;
  likeCount: number;
  subscriberCount: number;
  name: string;
  isPrivate: boolean;
  description: string;
  readme: string;
  ownerId: number;
}
