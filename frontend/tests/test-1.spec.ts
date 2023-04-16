import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'open drawer' }).click();
  await page.getByRole('button', { name: 'Journeys' }).click();
  await page.getByText('Count: 10').click();
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByRole('listitem').filter({ hasText: /^2$/ }).click();
  await page.getByText('Count: 10').click();
  await page.getByPlaceholder('Search').click();
  await page.getByPlaceholder('Search').fill('Pasila');
  await page.locator('#orderBy').selectOption('departureStationName');
  await page.locator('#limit').selectOption('50');
  await page.locator('#minDistance').click();
  await page.locator('#minDistance').fill('10');
  await page.locator('#maxDistance').click();
  await page.locator('#maxDistance').fill('100');
  await page.locator('#minDuration').click();
  await page.locator('#minDuration').fill('10');
  await page.locator('#maxDuration').click();
  await page.locator('#maxDuration').fill('100');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByText('Count: 50').click();
  await page.getByRole('button', { name: 'Page 3' }).click();
  await page.getByText('Count: 6').click();
  await page.getByRole('button', { name: 'Reset' }).click();
  await page.getByText('Count: 10').click();
});