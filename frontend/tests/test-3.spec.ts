import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'open drawer' }).click();
  await page.getByRole('button', { name: 'Stations' }).click();
  await page.locator('.css-1r9jet7 > .MuiButtonBase-root').click();
  await page.getByPlaceholder('Search').click();
  await page.getByPlaceholder('Search').fill('Pasila');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByRole('row', { name: '113 Pasilan asema Böle station Pasilan asema-aukio Böle station Helsinki Helsingfors CityBike Finland 40 60,1982108580315 24,932799079033 View' }).getByRole('button', { name: 'View' }).click();
  await page.getByRole('button', { name: 'Marker' }).click();
  await page.getByText('Name: Pasilan asema, Böle station Address: Pasilan asema-aukio, Böle station Cit').click();
  await page.locator('#filter').selectOption('march');
  await page.locator('#filter').selectOption('june');
  await page.getByRole('button', { name: 'Close popup' }).click();
  await page.getByText('113').click();
  await page.getByText('Pasilan asema', { exact: true }).click();
});