type Config = {
	PORT: number;
	DATABASE_URL: string;
	API_KEY: string;
};

function loadConfig(): Config {
	const { PORT, DATABASE_URL, API_KEY } = process.env;
	return {
		PORT: parseInt(PORT ?? '3000'),
		DATABASE_URL: DATABASE_URL ?? '',
		API_KEY: API_KEY || process.env.API_KEY || ''
	};
}
export const config = loadConfig();
