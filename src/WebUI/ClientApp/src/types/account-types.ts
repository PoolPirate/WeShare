export class AccountInfo {
  email: string;
  emailVerified: boolean;
}

export enum ServiceConnectionType {
  None = 0,

  Discord = 1,
}

export class ServiceConnectionSnippet {
  id: number;
  createdAt: Date;
  type: ServiceConnectionType;
}

export class DiscordServiceConnectionSnippet {
  id: number;
  createdAt: Date;
  type: ServiceConnectionType;
  discordId: number;
}
